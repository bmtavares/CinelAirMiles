namespace CinelAirMiles.Web.InternalAPI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Web.InternalAPI.Responses;

    public class MilesHelper : IMilesHelper
    {
        private readonly IMathHelper _mathHelper;
        private readonly IClientRepository _clientRepository;
        private readonly IMileRepository _mileRepository;

        public MilesHelper(
            IMathHelper mathHelper,
            IClientRepository clientRepository,
            IMileRepository mileRepository
            )
        {
            _mathHelper = mathHelper;
            _clientRepository = clientRepository;
            _mileRepository = mileRepository;
        }

        public double CalculateMiles(decimal depLat, decimal depLong, decimal arvLat, decimal arvLong)
        {
            var angle = _mathHelper.CalculateCentralSubtendedAngle(depLat, depLong, arvLat, arvLong);
            var km = _mathHelper.CalculateGreatCircleDistance(angle);

            return km / 1.609;
        }

        public async Task<int> TicketCheckerAsync(TicketList ticketList)
        {
            var res = 0;

            var flownTickets = ticketList.tickets;

            var flownNumbers = flownTickets.Select(t => t.MilesProgramNumber).Distinct();

            if (flownNumbers.Count() > 0)
            {
                var goldTierId = 3;

                foreach (string num in flownNumbers)
                {
                    var client = await _clientRepository.GetClientByNumberAsync(num);

                    if (client != null)
                    {
                        client = await _clientRepository.GetClientWithDetailsAsync(client.Id);

                        var firstNameNormalized = client.User.FirstName.ToUpper();
                        var lastNameNormalized = client.User.LastName.ToUpper();

                        var programTierMultiplier = GetProgramTierMultiplier(client.IsInReferrerProgram ? goldTierId : client.ProgramTierId);

                        var clientTickets = flownTickets.Where(t => t.MilesProgramNumber == num);

                        foreach (var ticket in clientTickets)
                        {
                            var ticketFirstNameNormalized = ticket.FirstName.ToUpper();
                            var ticketLastNameNormalized = ticket.LastName.ToUpper();

                            if (ticketFirstNameNormalized == firstNameNormalized && ticketLastNameNormalized == lastNameNormalized)
                            {
                                var baseMiles = CalculateMiles(ticket.DepartureLatitude, ticket.DepartureLongitude, ticket.ArrivalLatitude, ticket.ArrivalLongitude);
                                double classMultiplier = 0.10;

                                if (ticket.SeatClassId != 1)
                                {
                                    switch(ticket.SeatClassId)
                                    {
                                        case 2:
                                        case 3:
                                        case 4:
                                            classMultiplier = CalculateEconomicMultiplier(ticket.ArrivalRegion, ticket.DepartureRegion, ticket.SeatClassId);
                                            break;
                                        case 5:
                                            classMultiplier = 1.50;
                                            break;
                                        case 6:
                                            classMultiplier = 2;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                int finalMiles = (int)Math.Floor(baseMiles * (1 + classMultiplier + programTierMultiplier)); //TODO: Check if the house always wins

                                await CreditMilesToClient(client, finalMiles, ticket);

                                client.FlownSegments++;

                                await _clientRepository.UpdateAsync(client);

                                res++;
                            }
                        }
                    }

                }
            }

            return res;
        }

        async Task CreditMilesToClient(Client client, int miles, TicketResponse ticket)
        {
            var bonusMiles = new Mile
            {
                ClientId = client.Id,
                MilesTypeId = 2,
                Miles = miles,
                CreditDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(3),
                Description = $"Miles from flight that landed on {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")}"
            };

            var statusMiles = new Mile
            {
                ClientId = client.Id,
                MilesTypeId = 1,
                Miles = miles,
                CreditDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(3),
                Description = $"Status from flight that landed on {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")}"
            };

            await _mileRepository.CreateAsync(bonusMiles);
            await _mileRepository.CreateAsync(statusMiles);
        }

        double GetProgramTierMultiplier(int programTierId)
        {
            switch(programTierId)
            {
                case 2:
                    return 0.25;
                case 3:
                    return 0.5;
                default:
                    return 0;
            }
        }

        double CalculateEconomicMultiplier(int regionAId, int regionBId, int seatClassId)
        {
            if (IsIntercontinentalFlight(regionAId, regionBId))
            {
                switch (seatClassId)
                {
                    case 2:
                        return 0.5;
                    case 3:
                        return 1;
                    case 4:
                        return 1.5;
                }
            }
            switch (seatClassId)
            {
                case 2:
                    return 0.4;
                case 3:
                    return 0.7;
                case 4:
                    return 1;
            }

            return 0.10;
        }

        bool IsIntercontinentalFlight (int regionAId, int regionBId)
        {
            return regionAId != 1 && regionAId != 2 ? true :
                regionBId != 1 && regionBId != 2 ? true : false;
        }
    }
}

use CinelAirMiles;

select
	AspNetUsers.Email as 'User',
	iif(AspNetRoles.Name is not null, AspNetRoles.Name, 'No role (client)') as 'Role',
	iif(MilesProgramNumber is not null, MilesProgramNumber, 'Not a client') as 'Program number'
	from AspNetUsers
	left join AspNetUserRoles on Id = UserId
	left join AspNetRoles on AspNetRoles.Id = RoleId
	left join Clients on Clients.UserId = AspNetUsers.Id
	order by 'Role';


select
	Clients.Id as 'ID',
	Clients.MilesProgramNumber as 'Program number',
	Email as 'E-mail',
	ProgramTiers.Description as 'Description',
	AspNetUsers.PhoneNumber as 'Phone',
	AspNetUsers.FirstName as 'First name',
	AspNetUsers.LastName as 'Last name',
	Clients.FlownSegments as 'Flown segments',
	sum(Miles.Balance) as 'Total miles balance'
	from AspNetUsers
	right join Clients on Clients.UserId = AspNetUsers.Id
	left join ProgramTiers on ProgramTiers.Id = Clients.ProgramTierId
	left join Miles on Miles.ClientId = Clients.Id
	group by Clients.Id, Clients.MilesProgramNumber, Email, ProgramTiers.Description, AspNetUsers.PhoneNumber, AspNetUsers.FirstName, AspNetUsers.LastName, Clients.FlownSegments
	order by 'ID'
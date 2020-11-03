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
const http = require('http'); //carregamos o modulo
const express = require('express'),
  app = express(),
  mysql = require('mysql2'), // import mysql module
  bodyParser = require('body-parser');

const hostname = '192.168.193.7';
const port = 3000;

db = mysql.createConnection({
  host: '127.0.0.1',
  user: 'ticketbridge',
  password: '.0Lympüs',
  database: 'CinelAirTickets'
});

let server = http.createServer((req, res) => {

  console.log('URL:', req.url);
  console.log('METHOD:', req.method);

  switch(req.url) {

      case '/status':
      res.statusCode = 200;
      res.setHeader('Content-Type', 'text/html');
      res.end('Connection successful.');
      break;

      case'/list': 
          let sql = `SELECT Tickets.FirstName, Tickets.LastName, Tickets.MilesProgramNumber, Departure.Latitude AS 'DLat', Departure.Longitude AS 'DLong', Departure.RegionId AS 'DReg',
          Arrival.Latitude AS 'ALat', Arrival.Longitude AS 'ALong', Arrival.RegionId AS 'AReg', Tickets.SeatClassId
         FROM Tickets, Aeroports AS Departure, Aeroports AS Arrival
         WHERE Tickets.MilesProgramNumber IS NOT NULL
             AND (Tickets.AeroportDepartureId = Departure.Id AND Tickets.AeroportArrivalId = Arrival.Id) 
             AND (Tickets.DateArrival BETWEEN concat(DATE_ADD(CURDATE(), INTERVAL -1 DAY), ' 00:00') AND concat(curdate(), ' 00:00'));`;
          res.statusCode = 200;
          res.setHeader('Content-Type', 'application/json');
          db.query(sql, function(err, results, fields) {
            if (err) {
              console.log ('Error', err.message, err.stack);
              res.status(503).send('Service Unavailable');
              }
            res.end(JSON.stringify({results}));
          });
      break;

}});

server.listen(port, hostname, ()=>{
    console.log(`Ticket Bridge Server running at http://${hostname}:${port}/`);
});

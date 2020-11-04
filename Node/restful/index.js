const http = require('http'); //carregamos o módulo

const express = require('express'),
  app = express(),
  mysql = require('mysql2'), // import mysql module
  //cors = require('cors'),
  bodyParser = require('body-parser');

db = mysql.createConnection({
  host: '192.168.193.7',
  user: 'andreia',
  password: 'P@ssw0rd!',
  database: 'CinelAirTickets'
})

let server = http.createServer((req, res) => {

  console.log('URL:', req.url);
  console.log('METHOD:', req.method)

  switch(req.url) {

      case '/':
      res.statusCode = 200;
      res.setHeader('Content-Type', 'text/html');
      res.end('<h1>Olá</h1>')
      break;

      case '/users':
      res.statusCode = 200;
      res.setHeader('Content-Type', 'application/json');
      res.end(JSON.stringify({
          users: [{
                  name: 'Andreia',
                  email: 'andreia@example.org',
                  id:1
          }]
      }));
      break;

      case'/list': 
          let sql = `SELECT Tickets.MilesProgramNumber, Departure.Latitude AS 'DLat', Departure.Longitude AS 'DLong', Departure.RegionId AS 'DReg',
          Arrival.Latitude AS 'ALat', Arrival.Longitude AS 'ALong', Arrival.RegionId AS 'AReg', Tickets.SeatClassId
         FROM Tickets, Aeroports AS Departure, Aeroports AS Arrival
         WHERE Tickets.MilesProgramNumber IS NOT NULL
             AND (Tickets.AeroportDepartureId = Departure.Id AND Tickets.AeroportArrivalId = Arrival.Id) 
             AND (Tickets.DateArrival BETWEEN concat(DATE_ADD(CURDATE(), INTERVAL -1 DAY), ' 00:00') AND concat(curdate(), ' 00:00'));`;
          res.statusCode = 200;
          res.setHeader('Content-Type', 'application/json');
          db.query(sql, function(err, results, fields) {
            if (err) throw err;
            res.end(JSON.stringify({results}));
          });
      break;


}});

server.listen(3000, '127.0.0.1', ()=>{

    console.log('servidor a rodar!');
});
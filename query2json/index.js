var mysql = require('mysql');
const express = require('express');
const bodyParser = require('body-parser');

const app = express();
app.use(express.json());
app.use(bodyParser.urlencoded({ extended: true }));
const port = 3000;
var con = mysql.createPool({
	connectionLimit : 20,
	acquireTimeout  : 10000,
	host: "103.188.166.96",
	user: "admin",
	password: "Thuan@2022",
	database: 'Idgame'
});
app.post('/query2json', (req, res) => {
	const {sql, payload} = req.body;
	con.query(sql, payload, function (error, results, fields) {
		if (error)
		{
			res.status(400);
			res.json(error);
		}
		else{
			res.status(200);
			res.json(results);
		}
	});
})

app.listen(port, () => {
	console.log(`App listening on port ${port}`)
})

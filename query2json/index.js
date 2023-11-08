var mysql = require('mysql');
const express = require('express');
const bodyParser = require('body-parser');
const { exec } = require('child_process');


const app = express();
app.use(express.json());
app.use(bodyParser.urlencoded({ extended: true }));
const port = process.env.PORT || 3000;
var con = mysql.createPool({
	connectionLimit: 20,
	acquireTimeout: 10000,
	host: "103.188.166.96",
	user: "admin",
	password: "Thuan@2022",
	database: 'Idgame'
});
app.post('/query2json', (req, res) => {
	const { sql, payload } = req.body;
	con.query(sql, payload, function (error, results, fields) {
		if (error) {
			res.status(400);
			res.json(error);
		}
		else {
			res.status(200);
			res.json(results);
		}
	});
})

app.post('/block', (req, res) => {
	const { ip } = req.body;
	if (!ip) {
		res.status(200);
		res.end();
		return;
	}
	if (ip === '103.188.166.96') {
		res.status(200);
		res.end();
		return;
	}
	const key = `BLOCK IP ADDRESS - ${ip}`;
	const blockIPCommand = `netsh advfirewall firewall add rule name="${key}" dir=in action=block remoteip=${ip}`;

	exec(blockIPCommand, (error, stdout, stderr) => {
		if (error) {
			console.error(`Lỗi: ${error.message}`);
			res.status(400);
			res.json({
				message: error.message
			});
			return;
		}
		const output = stdout || stderr;
		console.log(`${key} : ${output.trim()}`);
		res.status(200);
		res.json({
			message: output
		});
		return;
	});
})

app.post('/unblock', (req, res) => {
	const { ip } = req.body;
	if (!ip) {
		res.status(400);
		res.end();
		return;
	}
	if (ip === '103.188.166.96') {
		res.status(400);
		res.end();
		return;
	}
	const key = `BLOCK IP ADDRESS - ${ip}`;
	const unblockIPCommand = `netsh advfirewall firewall delete rule name="${key}"`;

	exec(unblockIPCommand, (error, stdout, stderr) => {
		if (error) {
			console.error(`Lỗi: ${error.message}`);
			res.status(400);
			res.json({
				message: error.message
			});
			return;
		}
		const output = stdout || stderr;
		console.log(`RELEASE ${key} : ${output.trim()}`);
		res.status(200);
		res.json({
			message: output
		});
	});
})

app.listen(port, () => {
	console.log(`App listening on port ${port}`)
})

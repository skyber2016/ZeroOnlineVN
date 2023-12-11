const express = require('express');
const axios = require('axios');
const bodyParser = require('body-parser');
const { cryptoService } = require('./crypto')
const path = require('path');
const winston = require('winston');  require('winston-daily-rotate-file');
const {mid} = require('./mid');
const {fnAuthentication} = require('./authentication');

const app = express();
const port = 3000;
// Tạo một đối tượng transport DailyRotateFile
const dailyRotateFileTransport = new winston.transports.DailyRotateFile({
  filename: path.join('logs', '%DATE%.log'),
  datePattern: 'YYYY-MM-DD',
  zippedArchive: true,
  maxSize: '20m', // Kích thước tối đa cho mỗi file log
  maxFiles: '14d' // Số lượng file log được giữ lại
});
// Tạo logger
const logger = winston.createLogger({
  level: 'info', // Mức độ log tối thiểu
  format: winston.format.simple(),
  transports: [
    new winston.transports.Console(), // In log ra console
    dailyRotateFileTransport
  ]
});


app.use(bodyParser.json());

// Middleware to log requests
app.use((req, res, next) => {
  console.log(`FROM ${req.method} ${req.url} ${JSON.stringify(req.body)}`);
  next();
});

// Route to forward requests to a service using axios with .then and .catch
app.get('/', (req, res) => {
  const clientPublicKey = cryptoService.genPubKey();
  const encData = cryptoService.encrypt({ clientPublicKey, message: 'Response from Service' }, clientPublicKey);
  if (!encData) {
    return res.sendStatus(415);
  }
  return res.json(encData);
});

app.post('/', (req, res) => {
  const decData = cryptoService.decrypt(req.body);
  if (!decData) {
    return res.sendStatus(415);
  }
  const { clientPublicKey } = decData;
  return res.json(cryptoService.encrypt(decData, clientPublicKey));
});

app.post('/auth/login', (req, res) => {
  const token = fnAuthentication(req.body)
})

app.put('/', (req, res) => {
  const decData = cryptoService.decrypt(req.body);
  if (!decData) {
    return res.sendStatus(415);
  }
  const {type, message} = decData;
  // Ghi log
  logger.log(type, message);
  return res.sendStatus(200);
});


app.get('/public.PEM', (req, res) => {
  const pathToFile = path.resolve('rsa/public.PEM')
  return res.sendFile(pathToFile);
});


// Start the gateway
app.listen(port, () => {
  console.log(`Gateway is listening at http://localhost:${port}`);
});

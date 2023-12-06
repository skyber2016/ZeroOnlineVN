const express = require('express');
const axios = require('axios');
const bodyParser = require('body-parser');
const { cryptoService } = require('./crypto')
const path = require('path');
const app = express();
const port = 3000;
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
    return res.status(400).end();
  }
  return res.json(encData);
});

app.post('/', (req, res) => {
  const decData = cryptoService.decrypt(req.body);
  if (!decData) {
    return res.status(400).end();
  }
  const { clientPublicKey } = decData;
  return res.json(cryptoService.encrypt(decData, clientPublicKey));
});


app.get('/public.PEM', (req, res) => {
  const pathToFile = path.resolve('rsa/public.PEM')
  return res.sendFile(pathToFile);
});


// Start the gateway
app.listen(port, () => {
  console.log(`Gateway is listening at http://localhost:${port}`);
});

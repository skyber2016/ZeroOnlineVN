const jwt = require('jsonwebtoken');
const secretKey = 'yourSecretKey';

module.exports = {
    fnAuthentication: (username, password) => {
        return jwt.sign({username, password}, secretKey, { expiresIn: '1h' });
    }
}
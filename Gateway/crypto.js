const {Buffer} = require('buffer');
const forge = require('node-forge');
const fs = require('fs');

const cryptoService = {
    genPubKey: () => {
        // Tạo một đôi khóa mới (đôi khóa RSA với độ dài 1024 bits)
        const keyPair = forge.pki.rsa.generateKeyPair(1024);
        // Trích xuất khóa công khai
        const publicKeyPem = forge.pki.publicKeyToPem(keyPair.publicKey);
        return forge.util.encode64(publicKeyPem);
    },
    encrypt: (json, publicKeyBase64) => {
        try {
            // Step 1: Gen aes-256 key và iv (dạng binary)
            const aesKey = forge.random.getBytesSync(32);
            const iv = forge.random.getBytesSync(16);
      
            // thiết lập mode và mã hóa
            const cipher = forge.cipher.createCipher('AES-CBC', aesKey);
            cipher.start({iv: iv});
            const buffer = forge.util.createBuffer(
              forge.util.encodeUtf8(JSON.stringify(json))
            );
            cipher.update(buffer);
            cipher.finish();
            const byteArray = Buffer.from(buffer);
            // chuỗi final = iv + enc data
            const encryptedData = Buffer.concat([Buffer.from(iv, 'binary'), Buffer.from(cipher.output.data, 'binary')]);
      
            // convert dữ liệu finalBytes ra base64
            const PEM = forge.util.decode64(publicKeyBase64);
            const publicKey = forge.pki.publicKeyFromPem(PEM);
            const encryptedKey = publicKey.encrypt(forge.util.encode64(aesKey));
      
            return {
              d: encryptedData.toString('base64'),
              k: forge.util.encode64(encryptedKey)
            }
          } catch (error) {
            console.log(error);
            return null;
          }
      
    },
    decrypt: (input) => {
        try {
            const {k, d} = input;
            const PEM = fs.readFileSync('rsa/private.PEM');
            const privateKey = forge.pki.privateKeyFromPem(PEM);

            const aesKey = privateKey.decrypt(forge.util.decode64(k));

            const allBytes = Buffer.from(d, 'base64');

            // Tách dữ liệu, get iv và data từ server
            const iv = allBytes.slice(0, 16);
            const data = allBytes.slice(16);

            // Giải mã theo chuẩn AES/CTR/NoPadding
            const decipher = forge.cipher.createDecipher('AES-CBC', forge.util.decode64(aesKey));
            decipher.start({iv: iv.toString('binary')});
            decipher.update(forge.util.createBuffer(data));
            decipher.finish();

            const dec= forge.util.decodeUtf8(decipher.output.data);
            return JSON.parse(dec);
        } catch (error) {
            console.error(error);
            return null;
        }
    }
}

module.exports = {cryptoService}
How to create SSL certificate
1) Generate cert & validate `sudo certbot certonly --manual --preferred-challenges dns -d www.handmade-space.top `

2) Export to pfx format `sudo openssl pkcs12 -export -out /Users/kyeriomin/wildcard.pfx  -inkey privkey.pem -in cert.pem -certfile chain.pem`
3) Replace into cert directory
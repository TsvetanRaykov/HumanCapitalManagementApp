## Human Capital Management Application (HCM)

HCM App is a one-week project for demonstration purposes.

Used technologies:

- ASP .NET 6
  - WEB API + swagger
  - Razor Pages
  - Blazor Standalone WASM
  - MS Identity Framework
- Duende Identity Server
- PostgresSQL
- Docker, Docker compose
- Prometheus + cAdvisor
- Grafana



## Setup

Clone this repo on your Windows OS machine where Docker is up and running.

Open Power shell window and run this command from current directory of this repository.

```docker-compose up```



## Configuration

The following ports are exposed:

| Instance                          | Container      | Protocol   | Port | Alias                                                        |
| --------------------------------- | -------------- | ---------- | ---- | ------------------------------------------------------------ |
| PostgreSQL                        | db             | postgresql | 5432 | postgresql://localhost:5432/                                 |
| Duende Identity Server (HCM Auth) | identityserver | https      | 7000 | [https://localhost:7000](https://localhost:7000)             |
| HCM Api                           | api            | https      | 7001 | [https://localhost:7001/swagger](https://localhost:7001/swagger) |
| HCM App                           | app            | https      | 7003 | [https://localhost:7003](https://localhost:7003)             |
| Prometheus                        | prometheus     | http       | 9090 | [http://localhost:9090](http://localhost:9090)               |
| Grafana                           | grafana        | http       | 3000 | [http://localhost:3000](http://localhost:3000)               |
| cAdvisor                          | cadvisor       | http       | 8080 | [http://localhost:8080](http://localhost:8080)               |



## Notes

There is one pre-added user with admin role in main app (https://localhost:7003): 

*username:* tsvetan.raykov 

*password:* P@$$w0rd

Grafana dashboard (http://localhost:3000) can be accessed with default credentials (user/pass : admin/admin)

In case of issues with the used self-signed certificate, you can generate a new one by yourself:

```powershell
openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:identityserver,DNS:api,DNS:app"
```

```powershell
openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Humman Capital Management App"
```

Next, to avoid your browser complaining about an insecure connection you can add the certificate to the *Trusted Root Certification Authorities* of your current windows user. The password for the included in that repository certificate *localhost.pfx* is: U2Jw*nMyaW
This could be done either on double-clicking on localhost.pfx or with the following Power shell command:

```powershell
certutil -f -user -importpfx Root localhost.pfx
```

In addition would be good to add the lines below to the hosts file *(C:\Windows\System32\drivers\etc\hosts.txt)*

```powershell
127.0.0.1 app
127.0.0.1 api
127.0.0.1 identityserver
```


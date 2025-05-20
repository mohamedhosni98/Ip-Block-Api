#  IP Block API

A .NET Core Web API project to manage blocked countries and validate IP addresses using third-party geolocation APIs (e.g., [IPGeolocation.io](https://ipgeolocation.io/)).  
 The project uses **in-memory storage only** (no database).

---

##  Features

- ✅ Add / Remove blocked countries
- 📄 List all blocked countries with pagination & filtering
- 🌐 Lookup country by IP address
- 🛑 Automatically block IPs from blocked countries
- 📜 Log failed access attempts (country is blocked)
- ⏱️ Temporarily block a country for a specific duration
- 🧠 Background service to clear expired blocks
- 🧪 Swagger API documentation

---

##  Tech Stack

- ASP.NET Core 8 Web API
- In-Memory Collections (`ConcurrentDictionary`, `List`)
- `HttpClient` for third-party API requests
- `Newtonsoft.Json` for JSON parsing
- Swagger (Swashbuckle)

---

##  Setup Instructions

### 1. Clone the repository
```bash
git clone https://github.com/mohamedhosni98/Ip-Block-Api.git
cd IPBlockAPI

 API Endpoints
1.  Add Blocked Country
POST /api/countries/block
Body: { "countryCode": "US" }

2.  Delete Blocked Country
DELETE /api/countries/block/{countryCode}

3.  Get All Blocked Countries
GET /api/countries/blocked?page=1&pageSize=10&search=eg

4.  Lookup IP Info
GET /api/ip/lookup?ipAddress=8.8.8.8
If ipAddress is omitted, uses caller's IP.

5.  Check if IP is Blocked
GET /api/ip/check-block

Uses caller's IP automatically

Logs blocked attempts

6.  Get Blocked Attempts Log
GET /api/logs/blocked-attempts?page=1&pageSize=20

7.  Temporarily Block a Country
POST /api/countries/temporal-block
Body:

json

{
  "countryCode": "IR",
  "durationMinutes": 120
}

Background Services
A hosted service checks every 5 minutes to remove expired temporary blocks from memory.

📌 Notes
Project is self-contained, no database needed.

Designed for simplicity, clarity, and extendability.

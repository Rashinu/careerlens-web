# CareerLens — Monorepo

AI destekli kariyer ve maas analiz platformu. Turkiye pazari odakli.

## Yapi

```
careerlens-web/
├── frontend/   # Next.js 15 — Vercel'de deploy edilir
└── backend/    # ASP.NET Core 8 Web API — Azure'da deploy edilir
```

## Hizli Baslangic

### Frontend
```bash
cd frontend
npm install
npm run dev       # http://localhost:3000
```

### Backend
```bash
cd backend
docker compose up -d          # PostgreSQL + Redis
cd CareerLens.API
dotnet run --launch-profile http  # http://localhost:5100
```

## Tech Stack

| Katman | Teknoloji |
|---|---|
| Frontend | Next.js 15, Tailwind CSS v4, shadcn/ui |
| Backend | ASP.NET Core 8, CQRS + MediatR, EF Core 8 |
| AI | Semantic Kernel + OpenAI GPT |
| DB | PostgreSQL + Redis |
| Storage | Azure Blob Storage |
| Deploy | Vercel (frontend) + Azure App Service (backend) |

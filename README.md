# Mini-ERP (CRM & Facturation)

Proof of Concept d'un mini logiciel ERP développé dans le cadre d'une candidature
de stage chez **Proginov**, éditeur de solutions ERP.

## Stack technique

- **Back-end** : C# / .NET 8 (API REST)
- **ORM** : Entity Framework Core
- **Base de données** : PostgreSQL
- **Front-end** : React + Material UI

## Lancer le projet

### Prérequis
- .NET 8 SDK
- Node.js 18+
- PostgreSQL

### Base de données
1. Créer la BDD : `CREATE DATABASE minierp;`
2. Modifier la chaîne de connexion dans `backend/MiniERP.API/appsettings.json`
3. Appliquer les migrations : `dotnet ef database update`

### API Back-end
```bash
cd backend/MiniERP.API
dotnet run
```

### Front-end
```bash
cd frontend
npm install
npm run dev
```

## Architecture

Architecture N-Tier avec séparation des couches :
- **Controllers** : Points d'entrée API (routes HTTP)
- **Services** : Logique métier
- **Models** : Entités de données
- **Data** : Contexte Entity Framework et accès BDD

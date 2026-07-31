# Skillfrog Umbraco Demo

Training solution for the session **From Code to Production: Git, CI/CD, Deployments and Web Fundamentals for .NET and Umbraco**.

This repository is intentionally small so you can demonstrate:

- Git feature-branch and pull-request workflow
- GitHub Actions CI (restore → build → test → publish → artifact)
- Publishing a .NET / Umbraco app
- An Umbraco schema + view change using a **Short Description** product property

## Solution structure

```text
Skillfrog.Umbraco.sln
├── src/Skillfrog.Web              Umbraco 18 website (.NET 10, SQLite for local demos)
│   ├── Demo/ProductDescriptionFormatter.cs
│   └── Views/Product.cshtml       Product template used in the live walkthrough
└── tests/Skillfrog.Web.Tests      Unit tests exercised by CI
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A modern browser
- GitHub account (for PR + Actions demo)

## First-time local setup

Prefer **Kestrel** (`dotnet run`), not IIS Express — IIS Express is more likely to hit SQLite path/lock issues on first boot.

```powershell
cd d:\Training\skillfrog\umbraco
dotnet restore Skillfrog.Umbraco.sln
dotnet build Skillfrog.Umbraco.sln
dotnet test Skillfrog.Umbraco.sln
dotnet run --project src/Skillfrog.Web/Skillfrog.Web.csproj
```

1. Open the site URL from the console (default: `https://localhost:44301` or `http://localhost:35621`).
2. Development uses **unattended SQLite install**. On first run Umbraco creates `umbraco/Data/Umbraco.sqlite.db` automatically.
3. Open the Backoffice at `/umbraco` and sign in with:

| Field | Value |
| --- | --- |
| Email | `admin@skillfrog.local` |
| Password | `SkillfrogDemo123!` |

These credentials are local-demo only (see `appsettings.Development.json`). Do not reuse them outside this training machine.

## Prepare content before the live demo

Do this **before** the session so the 3-minute walkthrough stays focused.

### 1. Create a Product Document Type

In the Backoffice:

1. Settings → Document Types → create **Product**
2. Alias: `product` (must match `Views/Product.cshtml`)
3. Add a default template named **Product** (Umbraco should pick up `Views/Product.cshtml`)
4. Allow the Product type under the home / root structure you will use
5. Create one Product content node, for example **Trail Backpack**, and publish it

Do **not** add Short Description yet — that is the live change.

### 2. Confirm the page renders

Open the Product URL. You should see the product name and the empty-state hint for Short Description.

## Live demo: Short Description walkthrough

Use this exact flow during section 6 of the session.

```text
1. Create feature branch
2. Add the property in Umbraco
3. Confirm the view already reads shortDescription
4. Test locally
5. Commit the code (and any exported schema if you use uSync)
6. Push the branch
7. Create a pull request
8. Watch CI restore, build, test and publish
9. Review and merge into main
10. Talk through staging → approval → production
```

### Git commands

```powershell
git switch main
git pull
git switch -c feature/product-short-description
```

### Umbraco Backoffice change

1. Open Document Type **Product**
2. Add property:
   - Name: `Short Description`
   - Alias: `shortDescription`
   - Editor: Textarea (or Textstring)
3. Save the Document Type
4. Open the Product content node, enter a short description, Save and Publish
5. Refresh the front-end page — the description should appear

The view already contains:

```csharp
var shortDescription = Model.Value<string>("shortDescription");
var formattedDescription = Skillfrog.Web.Demo.ProductDescriptionFormatter.Format(shortDescription);
```

### Commit and push

```powershell
git status
git add .
git commit -m "Add product short description support"
git push -u origin feature/product-short-description
```

Then create a pull request on GitHub and open the **Actions** tab to show CI.

## Optional CI failure demo

To show “build green but test failure blocks deployment”:

1. On a throwaway branch, change `PipelineSmokeTests` to `Assert.True(false)`
2. Push and open a PR
3. Show the red GitHub Actions run
4. Revert the change (or close the PR)

## Useful commands for the session

| Goal | Command |
| --- | --- |
| Status | `git status` |
| New branch | `git switch -c feature/product-short-description` |
| Commit | `git commit -m "Add product short description support"` |
| Push | `git push -u origin feature/product-short-description` |
| Local publish | `dotnet publish src/Skillfrog.Web/Skillfrog.Web.csproj -c Release -o ./publish` |
| Run tests | `dotnet test Skillfrog.Umbraco.sln` |

## Publishing vs deployment (talking points)

- **Publish** (`dotnet publish`) creates the runnable output (DLLs, views, static files, `web.config` when targeting IIS).
- **Deploy** copies that artifact into an environment and starts/restarts the app.
- CI uploads the `skillfrog-web` artifact so you can explain deploying the **tested** build instead of rebuilding on the server.

## Umbraco deployment reminders

Treat these separately in discussion:

| Change type | Examples |
| --- | --- |
| Code | C#, views, CSS/JS, packages |
| Schema | Document Types, Data Types, templates |
| Content | Pages, text, media |

This demo mixes **schema** (`shortDescription` property) and **code** (formatter + view). Call that out during the walkthrough.

## Secrets

Do not commit production connection strings or passwords. Development uses local SQLite via `appsettings.Development.json`. Environment-specific secrets belong in hosting configuration or a secret store.

## Session mapping

| Session topic | What to show in this repo |
| --- | --- |
| Web fundamentals | Request a Product URL; inspect status codes in DevTools |
| Git workflow | Feature branch → commit → push → PR |
| CI | `.github/workflows/ci.yml` on the PR |
| Artifact | Actions → upload `skillfrog-web` |
| Publish vs deploy | Local `dotnet publish` vs deploying the artifact |
| Practical walkthrough | Short Description end-to-end |

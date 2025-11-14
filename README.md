# AuthLibrary

Libreria di autenticazione per il progetto **Cicli L'Avarizia**.  
Offre funzionalità essenziali per la verifica dell’esistenza di un’email utente e la generazione di **JWT Token**.

---

## 📌 Descrizione

**AuthLibrary** fornisce un sistema di autenticazione base che utilizza il database **AdventureWorksSecurityLT2019** per controllare se un’email è registrata.  
In caso positivo, viene generato un **JWT Token** che può essere utilizzato dall’applicazione per gestire autorizzazioni e sessioni utente.

---

## 🧩 Funzionalità

### ✔ Verifica Email
- Controlla se un’email esiste nel database

### ✔ Generazione JWT Token
- Genera un token firmato se l'email esiste
- Include nel token informazioni come:
  - Email utente

---

## 🗄 Dipendenze e Requisiti

- Installare **Microsoft.Data.SqlClient**
- Database **AdventureWorksSecurityLT2019**
- Configurazione JWT nell’applicazione (file `appsettings.json`)

---

## 🧷 Installazione

Aggiungi nel tuo `Program.cs`:

```csharp
// Add token settings
TokenSettings tokenSettings = new();
builder.Configuration.Bind(nameof(TokenSettings), tokenSettings);
builder.Services.AddSingleton(tokenSettings);

var connectionString = builder.Configuration.GetConnectionString("AdventureWorksSecurityLT2019")
    ?? throw new ArgumentException("Error: connection string not found");

builder.Services.AddScoped<SqlDbHandler>(sp =>
{
    var tokenSettings = sp.GetRequiredService<TokenSettings>();
    return new SqlDbHandler(connectionString, tokenSettings);
});

# 💡 Merchant Application Core
This repository contains the **presentation and application layers**
of a commercial .NET MAUI mobile application.

It is responsible for:
- Real-world MVVM structure
- Feature-based modular UI
- Clean separation between UI, domain, and use cases
- Testable business rules without runtime or billing logic

---

## 📦 Project Structure
```
├── .build/     # Build output for deployment.
├── docs/       # Documentation, architecture diagrams, API references, etc.
├── src/        # All source code is contained here.
│   ├── *.App.Mobile.Presentation/  # MAUI Views & ViewModels (public)
│   ├── *.Application/              # Use cases & capability interfaces
│   ├── *.Domain/                   # Pure business rules & policies
│   ├── *.Infrastructure.Free/      # Free capability implementations
├── .gitignore
├── README.md
```

## 🔒 What Is Intentionally Excluded

This repository does **NOT** include:
- Subscription enforcement
- Billing logic
- Licensing or entitlement checks
- Store-specific integrations
- Runtime composition

Those concerns are implemented in **private assemblies**
to protect commercial intellectual property.

## 🧠 Capability-Based Design

Premium and free features are separated using **capability interfaces**
and **dependency injection**, not feature flags.

Example:
```csharp
public interface IInventoryCapability
{
    int MaxItemCount { get; }
}
```

The free implementation is included here; premium implementations
exist in private repositories only.

***

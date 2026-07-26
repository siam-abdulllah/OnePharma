# 🏥 OnePharma - Pharmacy Management System

A comprehensive pharmacy management and dashboard application built with modern web technologies for inventory tracking, sales management, and reporting.

---

## 📋 Overview

OnePharma is a full-stack web application designed to streamline pharmacy operations including:
- Inventory management and tracking
- Sales and transaction management
- Pharmacy dashboard with analytics
- Real-time reporting and insights

---

## 🛠️ Technology Stack

| Layer | Technology |
|-------|------------|
| **Frontend** | JavaScript, Bootstrap, HTML/CSS |
| **Backend** | ASP.NET Core |
| **Database** | SQL Server / Database |
| **Architecture** | MVC Pattern |

---

## ✨ Features

- 📊 **Comprehensive Dashboard** - Real-time analytics and KPIs
- 📦 **Inventory Management** - Stock tracking and alerts
- 💰 **Sales Tracking** - Transaction history and reporting
- 🔍 **Search & Filter** - Advanced product and transaction search
- 📱 **Responsive Design** - Works on desktop and tablet devices
- 📈 **Reports** - Generate sales and inventory reports

---

## 🚀 Getting Started

### Prerequisites
- ASP.NET Core runtime
- SQL Server or compatible database
- Node.js (for frontend dependencies, if applicable)
- Visual Studio or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/siam-abdulllah/OnePharma.git
   cd OnePharma
   ```

2. **Setup Database**
   - Configure connection string in `appsettings.json`
   - Run migrations (if using Entity Framework Core)

3. **Build and Run**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

4. **Access Application**
   - Open your browser and navigate to `https://localhost:5001` (or the configured port)

---

## 📁 Project Structure

```
OnePharma/
├── Pharma_Dashboard/          # ASP.NET Core backend
│   ├── Controllers/           # API endpoints
│   ├── Models/                # Data models
│   ├── Views/                 # MVC Views
│   ├── wwwroot/               # Static files (CSS, JS, Bootstrap)
│   └── appsettings.json       # Configuration
├── Database/                  # Database scripts and migrations
└── README.md
```

---

## 🔧 Configuration

Edit `appsettings.json` to configure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=OnePharma;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 📖 Usage

### Basic Workflow
1. Login to the pharmacy dashboard
2. Navigate to Inventory section to manage stock
3. Record sales transactions
4. Generate reports for analysis
5. View real-time analytics on dashboard

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is provided as-is for educational and professional use.

---

## 👤 Author

**Siam Abdullah**
- GitHub: [@siam-abdulllah](https://github.com/siam-abdulllah)
- Portfolio: [View all projects](https://github.com/siam-abdulllah?tab=repositories)

---

## 📞 Support

For issues, questions, or suggestions:
- Open an issue on GitHub
- Check existing issues for solutions

---

## 🎯 Future Enhancements

- [ ] Mobile app integration
- [ ] Advanced analytics and ML-based predictions
- [ ] Multi-pharmacy support
- [ ] API rate limiting and security enhancements
- [ ] Automated notifications and alerts

---

**Last Updated:** 2026  
**Status:** Active Development

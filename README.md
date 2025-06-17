<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h1 align="center">FotoGen</h1>
  <p align="center">
    AI-powered custom model training and image generation platform
    <br />
    <a href="https://github.com/ips-ag/FotoGen"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://app-fotogenapp-ui-dev.azurewebsites.net/">View Demo</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#architecture">Architecture</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

FotoGen is an advanced AI-powered platform that enables users to create custom image generation models and generate personalized AI images. The application leverages Replicate's infrastructure for model training and integrates with Azure services for scalable cloud deployment.

### Key Features

* **Custom Model Training**: Upload your images and train personalized AI models
* **AI Image Generation**: Generate high-quality images using your trained custom models
* **Email Notifications**: Receive automated email notifications when model training completes
* **Secure Authentication**: Microsoft Entra ID integration for secure authentication
* **Cloud-native Architecture**: Built on Azure with scalable infrastructure
* **Modern UI/UX**: Responsive React frontend with Shadcn/UI components

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

**Frontend:**
* [![React][React.js]][React-url]
* [![TypeScript][TypeScript.js]][TypeScript-url]
* [![Vite][Vite.js]][Vite-url]
* [![TailwindCSS][TailwindCSS.com]][TailwindCSS-url]
* [![Shadcn/UI][ShadcnUI.com]][ShadcnUI-url]

**Backend:**
* [![.NET][.NET]][.NET-url]
* [![C#][CSharp.com]][CSharp-url]

**Infrastructure & AI:**
* [![Azure][Azure.com]][Azure-url]
* [![Replicate][Replicate.com]][Replicate-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

* **.NET 9.0 SDK**
  ```sh
  # Download from https://dotnet.microsoft.com/download/dotnet/9.0
  dotnet --version
  ```

* **Node.js (20+)**
  ```sh
  # Install Node.js from https://nodejs.org/
  node --version
  npm --version
  ```

* **Azure Account** with the following services:
  - Azure Storage Account
  - Azure Communication Services (for email notifications)
  - Microsoft Entra ID (for authentication)

* **Replicate Account** for AI model training and inference

### Installation

1. **Clone the repository**
   ```sh
   git clone https://github.com/ips-ag/FotoGen.git
   cd FotoGen
   ```

2. **Backend Setup**
   ```sh
   cd src/api
   dotnet restore
   ```

3. **Frontend Setup**
   ```sh
   cd src/app
   npm install
   ```

4. **Configure Environment Variables**

   Create `src/api/FotoGen.Api/secrets.json`:
   ```json
   {
     "Replicate": {
       "Token": "your_replicate_token",
       "Owner": "your_replicate_account_owner",
     },
     "AzureStorage": {
       "ConnectionString": "your_azure_storage_connection_string"
     },
     "Authentication": {
       "ClientId": "your_entra_id_client_id",
       "Authority": "your_entra_id_authority"
     }
   }
   ```

5. **Run the Application**

   Backend:
   ```sh
   cd src/api
   dotnet run --project FotoGen.Api
   ```

   Frontend:
   ```sh
   cd src/app
   npm run dev
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
## Usage

### Training a Custom Model

1. **Upload Images**: Upload 10-20 high-quality images of your subject
2. **Start Training**: Initiate the model training process on Replicate
3. **Receive Notification**: Get email notification when training completes

### Generating Images

1. **Enter Prompt**: Describe the image you want to generate using your trained model
2. **Generate**: Create AI images using your custom model
3. **Download**: Save generated images to your device

**Note**: You have one personal model available. Training a new model will overwrite your previous version.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ARCHITECTURE -->
## Architecture

FotoGen follows a clean architecture pattern with clear separation of concerns:

### Backend Structure
```
src/api/
├── FotoGen.Api/              # Web API layer
├── FotoGen.Application/      # Application logic & use cases
├── FotoGen.Domain/          # Domain entities & interfaces
└── FotoGen.Infrastructure/  # External services & data access
```

### Key Components

- **Clean Architecture**: Separation of concerns with dependency inversion
- **CQRS Pattern**: Command and Query Responsibility Segregation using MediatR
- **Background Services**: Automated model training monitoring and email notifications
- **Repository Pattern**: Data access abstraction
- **Azure Integration**: Storage, Communication Services, and Authentication

### Technology Stack

| Layer | Technology |
|-------|------------|
| Frontend | React + TypeScript + Vite + TailwindCSS |
| Backend | .NET 9 + ASP.NET Core Web API |
| Authentication | Microsoft Entra ID + MSAL |
| Storage | Azure Blob Storage |
| Email | Azure Communication Services |
| AI Platform | Replicate (Flux models) |
| Infrastructure | Azure Bicep templates |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

IPS AG - [@ips-ag](https://github.com/ips-ag)

Project Link: [https://github.com/ips-ag/FotoGen](https://github.com/ips-ag/FotoGen)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/ips-ag/FotoGen.svg?style=for-the-badge
[contributors-url]: https://github.com/ips-ag/FotoGen/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/ips-ag/FotoGen.svg?style=for-the-badge
[forks-url]: https://github.com/ips-ag/FotoGen/network/members
[stars-shield]: https://img.shields.io/github/stars/ips-ag/FotoGen.svg?style=for-the-badge
[stars-url]: https://github.com/ips-ag/FotoGen/stargazers
[issues-shield]: https://img.shields.io/github/issues/ips-ag/FotoGen.svg?style=for-the-badge
[issues-url]: https://github.com/ips-ag/FotoGen/issues
[license-shield]: https://img.shields.io/github/license/ips-ag/FotoGen.svg?style=for-the-badge
[license-url]: https://github.com/ips-ag/FotoGen/blob/main/LICENSE

[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[TypeScript.js]: https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white
[TypeScript-url]: https://www.typescriptlang.org/
[Vite.js]: https://img.shields.io/badge/Vite-646CFF?style=for-the-badge&logo=vite&logoColor=white
[Vite-url]: https://vitejs.dev/
[TailwindCSS.com]: https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white
[TailwindCSS-url]: https://tailwindcss.com/
[ShadcnUI.com]: https://img.shields.io/badge/shadcn%2Fui-000000?style=for-the-badge&logo=shadcnui&logoColor=white
[ShadcnUI-url]: https://ui.shadcn.com/
[.NET]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[.NET-url]: https://dotnet.microsoft.com/
[CSharp.com]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white
[CSharp-url]: https://docs.microsoft.com/en-us/dotnet/csharp/
[Azure.com]: https://img.shields.io/badge/Microsoft_Azure-0089D0?style=for-the-badge&logo=microsoft-azure&logoColor=white
[Azure-url]: https://azure.microsoft.com/
[Replicate.com]: https://img.shields.io/badge/Replicate-000000?style=for-the-badge&logo=replicate&logoColor=white
[Replicate-url]: https://replicate.com/

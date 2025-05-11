# 🏙️ Draw Your City

🎓 **Full-score research project at TUM (Technical University of Munich)**  
📱 **Developed for Android platform**  
🧠 **AI + AR-powered participatory tool for urban facade and street co-design**

## 🎥 Demo Video

[![Watch the Demo Video](https://img.youtube.com/vi/LO6UUpuLDlM/hqdefault.jpg)](https://www.youtube.com/watch?v=LO6UUpuLDlM)

> 🔗 Click the image above to watch the full demonstration on YouTube.

📦 [Download Demo Video (MP4)](https://github.com/RuijieThranduil/Draw-Your-City/releases/download/untagged-56ce0263d20fc3e9f581/AD.Pro.Version.mp4)
   [Download Demo Video (MP4)](https://github.com/RuijieThranduil/Draw-Your-City/releases/download/untagged-56ce0263d20fc3e9f581/Ui.Prototype.on-Site.testing.video.mp4)
   
## 📌 Project Overview

**Draw Your City** is a Unity-based interactive design tool that empowers citizens to participate in reimagining their urban environment.

The project responds to the decline of department stores and inactive city streets by enabling ordinary users to:
- Take photos of real buildings
- Sketch rough ideas
- Use AI to generate design proposals in real time
- Explore, rate, and discuss others’ designs in AR

This project was developed as part of the “Tooling Urban Futures” studio and received a **full score** at TUM.

---

## 📱 Android APK

This application is built for **Android mobile devices**.  
You can find the APK file in the `/Build/` folder or [GitHub Releases](../../releases) if available.

> ✅ Tested on Android 11 and above  
> ❗ Please enable “install from unknown sources” on your phone if needed

---

## 🎨 UI Design – `Form/` Folder

The `/Form/` directory contains all the **UI components and logic** used in the app, including:
- Co-Design interface (sketch, prompt input)
- AR-based Exploration and Visualization panels
- Rating system for Evaluation phase

Designed with Unity’s UI system to support mobile resolution and touch interaction.

---

## 🛠️ Technologies Used

- Unity (2022.3.x LTS)
- AR Foundation
- AI image generation (DALL·E via OpenAI API)
- C#
- Unity UI Toolkit
- GPS + Prompt-driven categorization

---

## ⚠️ Notes on AI Integration

The AI-driven image generation feature uses **OpenAI's DALL·E API**.  
You must provide your own API key to use it.

### 🔧 How to add your API key:
1. Navigate to `Assets/Scripts/`
2. Open the script handling AI requests (e.g. `AIRequest.cs`)
3. Find the placeholder and insert your key:

```csharp
string apiKey = "your-api-key-here";


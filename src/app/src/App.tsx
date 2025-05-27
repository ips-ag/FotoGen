import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import { LogoutButton } from "./auth/LogoutButton";
import { ProfileData } from "./auth/ProfileData";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <LogoutButton />
      <ProfileData />
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>FotoGen - AI Image Generator</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>count is {count}</button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">Start building your custom AI models and generate amazing images!</p>
    </>
  );
}

export default App;

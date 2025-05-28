import { useMsal } from "@azure/msal-react";
import { loginRequest } from "./authConfig";
import "./LoginButton.css";

export const LoginButton = () => {
  const { instance } = useMsal();

  const handleLogin = () => {
    instance.loginPopup(loginRequest).catch((e) => {
      console.error(e);
    });
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Welcome to FotoGen</h1>
        <p>Please sign in with your Microsoft account to continue</p>
        <button className="login-button" onClick={handleLogin}>
          <svg width="18" height="18" viewBox="0 0 18 18" xmlns="http://www.w3.org/2000/svg">
            <rect x="1" y="1" width="7" height="7" fill="#f25022" />
            <rect x="10" y="1" width="7" height="7" fill="#7fba00" />
            <rect x="1" y="10" width="7" height="7" fill="#00a4ef" />
            <rect x="10" y="10" width="7" height="7" fill="#ffb900" />
          </svg>
          Sign in with Microsoft
        </button>
      </div>
    </div>
  );
};

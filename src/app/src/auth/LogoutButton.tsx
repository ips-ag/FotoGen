import { useMsal } from "@azure/msal-react";
import "./LogoutButton.css";

export const LogoutButton = () => {
  const { instance, accounts } = useMsal();

  const handleLogout = () => {
    if (accounts.length > 0) {
      instance.clearCache();
      window.location.reload();
    }
  };

  return (
    <button className="logout-button" onClick={handleLogout}>
      Sign out
    </button>
  );
};

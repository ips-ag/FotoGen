import { useMsal } from "@azure/msal-react";
import "./LogoutButton.css";

export const LogoutButton = () => {
  const { instance } = useMsal();

  const handleLogout = () => {
    instance.logoutPopup().catch((e) => {
      console.error(e);
    });
  };

  return (
    <button className="logout-button" onClick={handleLogout}>
      Sign out
    </button>
  );
};

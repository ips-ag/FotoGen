import { useMsal } from "@azure/msal-react";
import { useEffect, useState } from "react";
import "./ProfileData.css";

interface User {
  name?: string;
  userPrincipalName?: string;
  displayName?: string;
}

export const ProfileData = () => {
  const { accounts } = useMsal();
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    if (accounts && accounts.length > 0) {
      // Get the first account (single tenant, so there should only be one)
      const account = accounts[0];
      setUser({
        name: account.name,
        userPrincipalName: account.username,
        displayName: account.name,
      });
    }
  }, [accounts]);

  if (!user) {
    return <div>Loading user data...</div>;
  }

  return (
    <div className="profile-data">
      <h3>Welcome, {user.displayName || user.name}!</h3>
      <p>Signed in as: {user.userPrincipalName}</p>
    </div>
  );
};

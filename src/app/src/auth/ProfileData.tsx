import { useMsal } from "@azure/msal-react";
import { useEffect, useState } from "react";
import "./ProfileData.css";

interface User {
  name?: string;
  email?: string;
  displayName?: string;
}

export const ProfileData = () => {
  const { accounts } = useMsal();
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    if (accounts && accounts.length > 0) {
      const account = accounts[0];
      const email = (account.idTokenClaims?.email as string) || account.username;
      setUser({
        name: account.name,
        email: email,
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
      <p>Signed in as: {user.email}</p>
    </div>
  );
};

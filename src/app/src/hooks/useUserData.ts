
import { useState, useEffect } from 'react';
import { useMsal } from '@azure/msal-react';

export interface User {
  name?: string;
  email?: string;
  displayName?: string;
  id?: string;
}

export const useUserData = () => {
  const { accounts, instance } = useMsal();
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    if (accounts && accounts.length > 0) {
      const account = accounts[0];
      const email = (account.idTokenClaims?.email as string) || account.username;
      const id = account.idTokenClaims?.sub as string;
      
      setUser({
        name: account.name,
        email: email,
        displayName: account.name,
        id: id
      });

      console.log('User data set from MSAL account');
    } else {
      setUser(null);
    }
  }, [accounts, instance]);

  return { user };
};

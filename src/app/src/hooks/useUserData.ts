
import { useMemo } from 'react';
import { useMsal } from '@azure/msal-react';

export interface User {
  name?: string;
  email?: string;
  displayName?: string;
  id?: string;
}

export const useUserData = () => {
  const { accounts } = useMsal();

  const user = useMemo<User | null>(() => {
    if (!accounts || accounts.length === 0) {
      return null;
    }

    const account = accounts[0];
    const email = (account.idTokenClaims?.email as string) || account.username;
    const id = account.idTokenClaims?.sub as string;

    return {
      name: account.name,
      email: email,
      displayName: account.name,
      id: id
    };
  }, [accounts]);

  return { user };
};

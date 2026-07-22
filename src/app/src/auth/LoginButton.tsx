
import { useMsal } from "@azure/msal-react";
import { useNavigate } from "react-router-dom";
import { loginRequest } from "./authConfig";
import { Button } from '@/components/ui/button';
import { Building } from 'lucide-react';

export const LoginButton = () => {
  const { instance } = useMsal();
  const navigate = useNavigate();

  const handleLogin = () => {
    console.log('=== LOGINBUTTON - HANDLE LOGIN START ===');
    console.log('LoginButton - Starting login process');
    
    // Store the current URL path in sessionStorage before login if not already set
    const currentPath = window.location.pathname;
    console.log('LoginButton - Current path:', currentPath);
    console.log('LoginButton - window.location.href:', window.location.href);
    console.log('LoginButton - Current sessionStorage before login:');
    for (let i = 0; i < sessionStorage.length; i++) {
      const key = sessionStorage.key(i);
      if (key) {
        console.log(`  - ${key}: ${sessionStorage.getItem(key)}`);
      }
    }
    
    if (currentPath.includes('/model/')) {
      console.log('=== LOGINBUTTON - MODEL PATH DETECTED ===');
      console.log('LoginButton - Found model path, setting redirectPath');
      const existingRedirectPath = sessionStorage.getItem('redirectPath');
      console.log('LoginButton - Existing redirectPath:', existingRedirectPath);
      // Always set the redirectPath for model paths, overwrite any existing value
      sessionStorage.setItem('redirectPath', currentPath);
      console.log('LoginButton - Set redirectPath to:', currentPath);
    } else {
      console.log('LoginButton - Not a model path, no redirectPath set');
    }
    
    console.log('LoginButton - Current redirectPath in sessionStorage:', sessionStorage.getItem('redirectPath'));
    console.log('LoginButton - About to start popup login');
    
    instance.loginPopup(loginRequest)
      .then(() => {
        console.log('=== LOGINBUTTON - LOGIN SUCCESS ===');
        // Check if there's a stored redirect path
        const redirectPath = sessionStorage.getItem('redirectPath');
        console.log('LoginButton - After login, redirectPath from sessionStorage:', redirectPath);
        console.log('LoginButton - redirectPath type:', typeof redirectPath);
        
        if (redirectPath) {
          console.log('=== LOGINBUTTON - REDIRECT PATH FOUND ===');
          console.log('LoginButton - Found redirect path, navigating to:', redirectPath);
          // Navigate to the stored path first
          console.log('LoginButton - About to navigate to:', redirectPath);
          navigate(redirectPath);
          console.log('LoginButton - Navigation initiated to:', redirectPath);
          // Clear it after navigation is initiated
          sessionStorage.removeItem('redirectPath');
          console.log('LoginButton - Cleared redirectPath from sessionStorage');
        } else {
          console.log('=== LOGINBUTTON - NO REDIRECT PATH ===');
          console.log('LoginButton - No redirect path found, navigating to /home');
          navigate('/home');
        }
      })
      .catch((e) => {
        console.error('=== LOGINBUTTON - LOGIN ERROR ===');
        console.error('LoginButton - Login error:', e);
      });
  };

  return (
    <Button 
      onClick={handleLogin}
      className="w-full text-white font-semibold py-6 text-lg transition-all duration-300 transform hover:scale-105 shadow-lg flex items-center justify-center gap-2"
      style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
    >
      <Building className="h-5 w-5" />
      Sign in with Microsoft Entra ID
    </Button>
  );
};


import React, { useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { useIsAuthenticated } from '@azure/msal-react';
import Layout from '@/components/Layout';
import HomePage from './HomePage';

const Index = () => {
  const navigate = useNavigate();
  const isAuthenticated = useIsAuthenticated();
  const location = useLocation();
  const { username, modelName } = useParams();
  const path = location.pathname;

  console.log('=== INDEX.TSX - ENTRY ===');
  console.log('Index.tsx - Current path:', path);
  console.log('Index.tsx - isAuthenticated:', isAuthenticated);
  console.log('Index.tsx - username:', username, 'modelName:', modelName);
  console.log('Index.tsx - sessionStorage before any logic:', sessionStorage.getItem('redirectPath'));
  console.log('Index.tsx - All sessionStorage items:');
  for (let i = 0; i < sessionStorage.length; i++) {
    const key = sessionStorage.key(i);
    if (key) {
      console.log(`  - ${key}: ${sessionStorage.getItem(key)}`);
    }
  }

  useEffect(() => {
    console.log('=== INDEX.TSX - USEEFFECT START ===');
    console.log('Index.tsx useEffect - path:', path, 'isAuthenticated:', isAuthenticated);
    console.log('Index.tsx useEffect - username:', username, 'modelName:', modelName);
    
    // Check if URL path contains '/model/'
    const isModelPath = path.includes('/model/');
    console.log('Index.tsx - isModelPath:', isModelPath);

    if (!isAuthenticated) {
      console.log('=== UNAUTHORIZED USER FLOW ===');
      // Store the intended path if it's a model path
      if (isModelPath) {
        console.log('Index.tsx - User not authenticated, storing model path in sessionStorage:', path);
        console.log('Index.tsx - SessionStorage before storing:', sessionStorage.getItem('redirectPath'));
        
        // Always set the redirectPath for model paths, overwrite any existing value
        sessionStorage.setItem('redirectPath', path);
        console.log('Index.tsx - SessionStorage after storing:', sessionStorage.getItem('redirectPath'));

        // Log all session storage items for debugging
        console.log('Index.tsx - All sessionStorage items after storing:');
        for (let i = 0; i < sessionStorage.length; i++) {
          const key = sessionStorage.key(i);
          if (key) {
            console.log(`  - ${key}: ${sessionStorage.getItem(key)}`);
          }
        }
        
        console.log('Index.tsx - About to navigate to /login for model path:', path);
      } else {
        console.log('Index.tsx - User not authenticated, non-model path, no redirect path stored');
      }
      
      console.log('Index.tsx - Not authenticated, redirecting to /login');
      navigate('/login');
      return;
    }

    console.log('=== AUTHENTICATED USER FLOW ===');
    // User is authenticated - handle different path types
    if (isModelPath) {
      console.log('Index.tsx - Authenticated user on model path, will render HomePage component');
      console.log('Index.tsx - Model path details - username:', username, 'modelName:', modelName);
      // Clear any stored redirect path since we're now on the intended page
      const storedPath = sessionStorage.getItem('redirectPath');
      console.log('Index.tsx - Clearing stored redirectPath:', storedPath);
      sessionStorage.removeItem('redirectPath');
      // Don't redirect, let the component render HomePage
      return;
    } else if (path === '/') {
      console.log('Index.tsx - On root path, redirecting to /home');
      // Only redirect to home if we're on the root path
      navigate('/home');
    } else {
      console.log('Index.tsx - On other path, no redirect needed:', path);
    }
    // If we're already on /home or other paths, don't redirect
  }, [navigate, isAuthenticated, path, username, modelName]);

  // Show loading while authentication state is being determined
  if (isAuthenticated === undefined) {
    console.log('Index.tsx - Authentication state undefined, showing loading');
    return (
      <Layout>
        <div className="flex items-center justify-center min-h-screen">
          <div className="text-center">
            <h1 className="text-2xl font-bold text-gray-900 mb-2">Loading...</h1>
            <p className="text-gray-600">Checking authentication status</p>
          </div>
        </div>
      </Layout>
    );
  }

  // If authenticated and on a model path, render HomePage
  if (isAuthenticated && path.includes('/model/')) {
    console.log('=== RENDERING HOMEPAGE FOR MODEL PATH ===');
    console.log('Index.tsx - Rendering HomePage for authenticated user on model path');
    console.log('Index.tsx - Final render - username:', username, 'modelName:', modelName, 'path:', path);
    return <HomePage />;
  }

  // If authenticated and on home, redirect to /home route
  if (isAuthenticated && path === '/home') {
    console.log('Index.tsx - Returning null for authenticated user on home path');
    return null;
  }

  console.log('Index.tsx - Fallback render - showing loading/redirecting message');
  return (
    <Layout>
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-2">Loading...</h1>
          <p className="text-gray-600">Redirecting you to the appropriate page</p>
        </div>
      </div>
    </Layout>
  );
};

export default Index;

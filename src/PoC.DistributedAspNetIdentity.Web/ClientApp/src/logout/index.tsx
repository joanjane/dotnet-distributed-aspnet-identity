import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../core/auth/store';

export function Logout(): JSX.Element {
  const logout = useAuthStore(state => state.logout);
  const navigate = useNavigate();

  useEffect(() => {
    async function logoutAndRedirect() {
      await logout();
      navigate('/');
    }

    logoutAndRedirect();
  }, [navigate, logout]);

  return null;
}
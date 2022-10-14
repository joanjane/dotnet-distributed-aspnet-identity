import { useEffect } from 'react';
import { useAuthStore } from './store';

const checkSessionMinutes = 2;
export function useCheckSession() {
  const checkSession = useAuthStore(state => state.checkSession);

  useEffect(() => {
    checkSession();

    const interval = setInterval(() => {
      checkSession();
    }, checkSessionMinutes * 60 * 1000);

    return () => {
      console.log('Clearing interval');
      clearInterval(interval);
    };
  }, [checkSession]);
}
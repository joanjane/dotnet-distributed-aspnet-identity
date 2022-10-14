import { PropsWithChildren } from 'react';

import { Login } from '../../login';
import { useAuthStore } from './store';

export function AuthGuard({ children }: PropsWithChildren) {
  const { isAuthenticated, loaded } = useAuthStore(state => state.auth);

  if (!loaded) {
    return <>Loading...</>;
  }

  if (!isAuthenticated) {
    return <Login />;
  }

  return <>{children}</>;
}

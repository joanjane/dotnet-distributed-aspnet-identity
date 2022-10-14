import create from 'zustand';
import { HttpError, UnknownHttpError } from '../api/errors';

export type User = {
  id: string;
  email: string;
  name: string;
  surname: string;
};

type UserStoreState = {
  isAuthenticated: boolean,
  loaded: boolean,
  user?: User
};

type UserStore = {
  auth: UserStoreState,
  checkSession: () => Promise<void>,
  login: (credentials: { email: string, password: string }) => Promise<void>,
  logout: () => Promise<void>
}

const initialState: UserStoreState = {
  isAuthenticated: false,
  loaded: false,
  user: null
};
export const useAuthStore = create<UserStore>((set) => ({
  auth: initialState,
  checkSession: async () => {
    const response = await fetch('/api/auth/check-session');
    if (!response.ok) {
      console.log('Check session failed');
      set({ auth: { loaded: true, isAuthenticated: false, user: null } })
      return;
    }

    console.log('Check session succeeded');
    set({ auth: { loaded: true, isAuthenticated: true, user: await response.json() } })
  },
  login: async (credentials: { email: string, password: string }) => {
    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(credentials)
    });

    if (!response.ok) {
      set({ auth: { loaded: true, isAuthenticated: false, user: null } })

      if (response.status < 500) {
        throw new HttpError(response.status, await response.json());
      }
      throw new UnknownHttpError(response, 'Login error');
    }

    set({ auth: { loaded: true, isAuthenticated: true, user: await response.json() } })
  },
  logout: async () => {
    const response = await fetch('/api/auth/logout', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      }
    });

    if (!response.ok) {
      throw new UnknownHttpError(response, 'Logout error');
    }

    set({ auth: { loaded: true, isAuthenticated: false, user: null } })
  }
}));


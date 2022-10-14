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

export type LoginRequest = {
  email: string;
  password: string;
};

export type SignupRequest = {
  email: string;
  password: string;
  name: string;
  surname: string;
};

type UserStore = {
  auth: UserStoreState,
  checkSession: () => Promise<void>,
  login: (credentials: LoginRequest) => Promise<void>,
  signup: (request: SignupRequest) => Promise<void>,
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
  login: async (credentials: LoginRequest) => {
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
  signup: async (request: SignupRequest) => {
    const response = await fetch('/api/auth/signup', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
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


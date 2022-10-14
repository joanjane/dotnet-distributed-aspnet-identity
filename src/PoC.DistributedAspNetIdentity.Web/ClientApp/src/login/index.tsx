import './index.scss';

import { FormEvent, useCallback, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../core/auth/store';
import { HttpError } from '../core/api/errors';

export function Login() {
  const [errors, setErrors] = useState<string[]>([]);
  const [formState, setFormState] = useState({ email: '', password: '' });
  const login = useAuthStore(state => state.login);
  const navigate = useNavigate();
  const location = useLocation();
  const isLoginRoute = location.pathname === '/login';
  const isAuthenticated = useAuthStore(state => state.auth.isAuthenticated);

  useEffect(() => {
    if (isAuthenticated && isLoginRoute) {
      navigate('/');
    }
  }, [navigate, isAuthenticated, isLoginRoute]);

  const onSubmit = useCallback(async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrors([]);
    try {
      await login(formState);
    } catch (e) {
      if (e instanceof HttpError) {
        setErrors(e.errorCodes.map(e => `Error ${e}`));
      } else {
        setErrors(['Unknown error']);
        console.error(e);
      }
    }
  }, [formState, login]);

  return (
    <form className="app-login" onSubmit={onSubmit}>
      <h1 className="app-login__title">Login to continue</h1>
      <div className="app-form-control">
        <label htmlFor="login-email">Email</label>
        <input
          type="email"
          placeholder="user@email.com"
          value={formState.email}
          onChange={(e) => setFormState(prev => ({...prev, email: e.target.value}))} />
      </div>

      <div className="app-form-control">
        <label htmlFor="login-password">Password</label>
        <input
          type="password"
          placeholder="*****"
          value={formState.password}
          onChange={(e) => setFormState(prev => ({...prev, password: e.target.value}))} />
      </div>

      {errors.length > 0 &&
      <ul className="app-form-errors">
        {errors.map((error, i) => <li key={i}>{error}</li>)}
      </ul>}

      <button className="app-btn">Login</button>
    </form>
  );
}
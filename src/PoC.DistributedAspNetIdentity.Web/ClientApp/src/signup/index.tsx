import { FormEvent, useCallback, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { SignupRequest, useAuthStore } from '../core/auth/store';
import { HttpError } from '../core/api/errors';

export function Signup() {
  const [errors, setErrors] = useState<string[]>([]);
  const [formState, setFormState] = useState<SignupRequest>({ email: '', password: '', name: '', surname: '' });
  const signup = useAuthStore(state => state.signup);
  const navigate = useNavigate();

  const onSubmit = useCallback(async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrors([]);
    try {
      await signup(formState);
      navigate('/');
    } catch (e) {
      if (e instanceof HttpError) {
        setErrors(e.errorCodes.map(e => `Error ${e}`));
      } else {
        setErrors(['Unknown error']);
        console.error(e);
      }
    }
  }, [formState, navigate, signup]);

  return (
    <form className="app-login" onSubmit={onSubmit}>
      <h1 className="app-login__title">Sign up</h1>
      <div className="app-form-control">
        <label htmlFor="login-email">Email</label>
        <input
          id="login-email"
          type="email"
          placeholder="user@email.com"
          value={formState.email}
          onChange={(e) => setFormState(prev => ({...prev, email: e.target.value}))} />
      </div>

      <div className="app-form-control">
        <label htmlFor="login-password">Password</label>
        <input
          id="login-password"
          type="password"
          placeholder="*****"
          value={formState.password}
          onChange={(e) => setFormState(prev => ({...prev, password: e.target.value}))} />
      </div>

      <div className="app-form-control">
        <label htmlFor="login-name">Name</label>
        <input
          id="login-name"
          type="text"
          placeholder="John"
          value={formState.name}
          onChange={(e) => setFormState(prev => ({...prev, name: e.target.value}))} />
      </div>

      <div className="app-form-control">
        <label htmlFor="login-surname">Surname</label>
        <input
          id="login-surname"
          type="text"
          placeholder="Doe"
          value={formState.surname}
          onChange={(e) => setFormState(prev => ({...prev, surname: e.target.value}))} />
      </div>

      {errors.length > 0 &&
      <ul className="app-form-errors">
        {errors.map((error, i) => <li key={i}>{error}</li>)}
      </ul>}

      <button className="app-btn">Sign up</button>
    </form>
  );
}
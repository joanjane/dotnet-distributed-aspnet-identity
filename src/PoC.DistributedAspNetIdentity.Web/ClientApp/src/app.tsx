import { useCallback } from 'react';
import { Route, Routes } from 'react-router-dom';
import { AuthGuard } from './core/auth/guard';
import { useCheckSession } from './core/auth/hooks';
import { useAuthStore } from './core/auth/store';
import { Login } from './login';
import { Logout } from './logout';
import { Layout } from './shared/layout';
import { Signup } from './signup';

function App() {
  useCheckSession();

  return (
    <Routes>
      <Route path="/" element={
        <AuthGuard>
          <Layout>
            <Home />
          </Layout>
        </AuthGuard>
      } />

      <Route path="/profile" element={
        <AuthGuard>
          <Layout>
            <MyProfile />
          </Layout>
        </AuthGuard>
      } />

      <Route path="login" element={<Login />} />
      <Route path="signup" element={<Signup />} />
      <Route path="logout" element={<Logout />} />
      <Route path="*" element={
        <Layout>
          <NotFound />
        </Layout>
      } />
    </Routes>
  );
}

function Home() {
  const testHomeCall = useCallback(async () => {
    const response = await fetch('/api/home');
    console.log(await response.json());
  }, []);

  return (
    <div className="app-content">
      <h1>Home</h1>
      <button onClick={testHomeCall}>Test authenticated request</button>
    </div>
  );
}

function MyProfile() {
  const user = useAuthStore(state => state.auth.user);
  return (<div className="app-content">
    <h1>My profile</h1>
    <div>
      <div><strong>Email:</strong></div>
      <div>{user?.email}</div>
    </div>

    <div>
      <div><strong>Name:</strong></div>
      <div>{user?.name}</div>
    </div>

    <div>
      <div><strong>Surname:</strong></div>
      <div>{user?.surname}</div>
    </div>
  </div>);
}


function NotFound() {
  return (
    <div className="app-content">
      <h1>Not found</h1>
    </div>
  );
}

export default App;

import './index.scss';
import { PropsWithChildren } from 'react';
import { Link } from 'react-router-dom';

export function Layout({ children }: PropsWithChildren) {
  return <>
    <Nav />
    <main className="app-main">{children}</main>
    <Footer />
  </>;
}

function Nav() {
  return (
    <nav className="app-nav">
      <Link to="/" className="app-nav__link">Home</Link>
      <Link to="/profile" className="app-nav__link">My profile</Link>
      <Link to="/logout" className="app-nav__link">Logout</Link>
    </nav>
  );
}

function Footer() {
  return (
    <footer className="app-footer">
      PoC.DistributedAspNetIdentity.Web
    </footer>
  );
}

import { render, screen } from '@testing-library/react';
import { Login } from '.';

test('renders learn react link', () => {
  render(<Login />);
  const linkElement = screen.getByText(/login/i);
  expect(linkElement).toBeInTheDocument();
});

import React from 'react';
import { Outlet } from 'react-router-dom';

const styles = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    flexDirection: 'column',
    fontFamily: 'Arial, sans-serif',
    background: '#f1f3f5', // jasne tło całej strony
  },
  navbar: {
    background: '#0d6efd', // niebieski
    color: '#ffffff',
    padding: '1rem 1.5rem',
    fontSize: '1.125rem',
  },
  main: {
    flex: 1,
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: '2rem 1.5rem',
  },
  formContainer: {
    background: '#ffffff',
    padding: '2.5rem 2rem',
    borderRadius: '0.5rem',
    boxShadow: '0 0.125rem 0.25rem rgba(0, 0, 0, 0.075)',
    width: '100%',
    maxWidth: '400px',
  },
  formTitle: {
    fontSize: '1.5rem',
    fontWeight: 'bold',
    marginBottom: '1.5rem',
    color: '#212529',
  },
  footer: {
    background: '#212529', // ciemny footer
    color: '#ffffff',
    padding: '0.75rem 1.5rem',
    textAlign: 'center',
  },
};

const AuthLayout = () => {
  return (
    <div style={styles.container}>
      <header style={styles.navbar}></header>
      <main style={styles.main}>
        <div style={styles.formContainer}>
          <h1 style={styles.formTitle}></h1>
          <Outlet />
        </div>
      </main>
      <footer style={styles.footer}></footer>
    </div>
  );
};

export default AuthLayout;

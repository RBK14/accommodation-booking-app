import React from 'react';
import { Outlet } from 'react-router-dom';

const styles = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    flexDirection: 'column',
    fontFamily: 'Arial, sans-serif',
  },
  navbar: {
    background: '#0d6efd', // niebieski
    color: '#ffffff',
    padding: '1rem 1.5rem',
    fontSize: '1.125rem',
  },
  main: {
    flex: 1,
    background: '#f1f3f5', // jasne tło body
    padding: '2rem 1.5rem',
  },
  footer: {
    background: '#212529', // ciemny footer
    color: '#ffffff',
    padding: '0.75rem 1.5rem',
    textAlign: 'center',
  },
};

const MainLayout = () => {
  return (
    <div style={styles.container}>
      <header style={styles.navbar}></header>
      <main style={styles.main}>
        <Outlet />
      </main>
      <footer style={styles.footer}></footer>
    </div>
  );
};

export default MainLayout;

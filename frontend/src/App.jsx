import { BrowserRouter, Routes, Route, Link, useLocation } from 'react-router-dom';
import {
  CssBaseline, AppBar, Toolbar, Typography, Button, Box
} from '@mui/material';
import ClientsPage from './pages/ClientsPage';
import FacturesPage from './pages/FacturesPage';

function NavBar() {
  const location = useLocation();

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" sx={{ mr: 4 }}>
          Mini-ERP
        </Typography>
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Button
            color="inherit"
            component={Link}
            to="/"
            sx={{
              borderBottom: location.pathname === '/' ? '2px solid white' : 'none'
            }}
          >
            Clients
          </Button>
          <Button
            color="inherit"
            component={Link}
            to="/factures"
            sx={{
              borderBottom: location.pathname === '/factures' ? '2px solid white' : 'none'
            }}
          >
            Factures
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
}

function App() {
  return (
    <BrowserRouter>
      <CssBaseline />
      <NavBar />
      <Routes>
        <Route path="/" element={<ClientsPage />} />
        <Route path="/factures" element={<FacturesPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
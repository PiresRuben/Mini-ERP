import { useState, useEffect } from 'react';
import api from '../api';
import {
  Container, Typography, Button, Table, TableBody, TableCell,
  TableContainer, TableHead, TableRow, Paper, IconButton, Box,
  Chip, Dialog, DialogTitle, DialogContent, DialogActions,
  FormControl, InputLabel, Select, MenuItem
} from '@mui/material';
import { Delete, Visibility } from '@mui/icons-material';
import FactureDetail from '../components/FactureDetail';

export default function FacturesPage() {
  const [factures, setFactures] = useState([]);
  const [clients, setClients] = useState([]);
  const [selectedFacture, setSelectedFacture] = useState(null);
  const [showCreate, setShowCreate] = useState(false);
  const [newClientId, setNewClientId] = useState('');

  useEffect(() => {
    loadFactures();
    loadClients();
  }, []);

  const loadFactures = async () => {
    try {
      const response = await api.get('/api/factures');
      setFactures(response.data);
    } catch (error) {
      console.error('Erreur chargement factures:', error);
    }
  };

  const loadClients = async () => {
    try {
      const response = await api.get('/api/clients');
      setClients(response.data);
    } catch (error) {
      console.error('Erreur chargement clients:', error);
    }
  };

  const handleCreate = async () => {
    try {
      await api.post('/api/factures', { clientId: newClientId });
      setShowCreate(false);
      setNewClientId('');
      loadFactures();
    } catch (error) {
      console.error('Erreur création facture:', error);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Supprimer cette facture ?')) return;
    try {
      await api.delete(`/api/factures/${id}`);
      loadFactures();
    } catch (error) {
      console.error('Erreur suppression:', error);
    }
  };

  const getStatutColor = (statut) => {
    switch (statut) {
      case 'Brouillon': return 'default';
      case 'Envoyée': return 'primary';
      case 'Payée': return 'success';
      case 'Annulée': return 'error';
      default: return 'default';
    }
  };

  // Si une facture est sélectionnée, afficher le détail
  if (selectedFacture) {
    return (
      <FactureDetail
        factureId={selectedFacture}
        onBack={() => {
          setSelectedFacture(null);
          loadFactures();
        }}
      />
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Factures</Typography>
        <Button variant="contained" onClick={() => setShowCreate(true)}>
          + Nouvelle Facture
        </Button>
      </Box>

      {/* Dialog de création */}
      <Dialog open={showCreate} onClose={() => setShowCreate(false)}>
        <DialogTitle>Nouvelle Facture</DialogTitle>
        <DialogContent sx={{ minWidth: 300, pt: 2 }}>
          <FormControl fullWidth sx={{ mt: 1 }}>
            <InputLabel>Client</InputLabel>
            <Select
              value={newClientId}
              label="Client"
              onChange={(e) => setNewClientId(e.target.value)}
            >
              {clients.map((c) => (
                <MenuItem key={c.id} value={c.id}>
                  {c.nomEntreprise}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowCreate(false)}>Annuler</Button>
          <Button
            variant="contained"
            onClick={handleCreate}
            disabled={!newClientId}
          >
            Créer
          </Button>
        </DialogActions>
      </Dialog>

      {/* Tableau des factures */}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
              <TableCell><strong>N° Facture</strong></TableCell>
              <TableCell><strong>Client</strong></TableCell>
              <TableCell><strong>Date</strong></TableCell>
              <TableCell><strong>Statut</strong></TableCell>
              <TableCell align="right"><strong>Total TTC</strong></TableCell>
              <TableCell><strong>Actions</strong></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {factures.map((facture) => (
              <TableRow key={facture.id}>
                <TableCell>{facture.numeroFacture}</TableCell>
                <TableCell>{facture.nomClient}</TableCell>
                <TableCell>
                  {new Date(facture.dateEmission).toLocaleDateString('fr-FR')}
                </TableCell>
                <TableCell>
                  <Chip
                    label={facture.statut}
                    color={getStatutColor(facture.statut)}
                    size="small"
                  />
                </TableCell>
                <TableCell align="right">
                  {facture.totalTTC.toFixed(2)} €
                </TableCell>
                <TableCell>
                  <IconButton
                    color="primary"
                    onClick={() => setSelectedFacture(facture.id)}
                  >
                    <Visibility />
                  </IconButton>
                  <IconButton
                    color="error"
                    onClick={() => handleDelete(facture.id)}
                  >
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
            {factures.length === 0 && (
              <TableRow>
                <TableCell colSpan={6} align="center">
                  Aucune facture pour le moment
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
}
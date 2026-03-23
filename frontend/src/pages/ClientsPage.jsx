import { useState, useEffect } from 'react';
import api from '../api';
import {
  Container, Typography, Button, Table, TableBody, TableCell,
  TableContainer, TableHead, TableRow, Paper, IconButton, Box
} from '@mui/material';
import { Delete, Edit } from '@mui/icons-material';
import ClientForm from '../components/ClientForm';

export default function ClientsPage() {
  const [clients, setClients] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [editingClient, setEditingClient] = useState(null);

  // Charger les clients au démarrage
  useEffect(() => {
    loadClients();
  }, []);

  const loadClients = async () => {
    try {
      const response = await api.get('/api/clients');
      setClients(response.data);
    } catch (error) {
      console.error('Erreur chargement clients:', error);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Supprimer ce client ?')) return;
    try {
      await api.delete(`/api/clients/${id}`);
      loadClients(); // Recharger la liste
    } catch (error) {
      console.error('Erreur suppression:', error);
    }
  };

  const handleEdit = (client) => {
    setEditingClient(client);
    setShowForm(true);
  };

  const handleFormClose = () => {
    setShowForm(false);
    setEditingClient(null);
    loadClients(); // Recharger après ajout/modif
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Clients</Typography>
        <Button
          variant="contained"
          onClick={() => setShowForm(true)}
        >
          + Nouveau Client
        </Button>
      </Box>

      {showForm && (
        <ClientForm
          client={editingClient}
          onClose={handleFormClose}
        />
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
              <TableCell><strong>Entreprise</strong></TableCell>
              <TableCell><strong>SIRET</strong></TableCell>
              <TableCell><strong>Adresse</strong></TableCell>
              <TableCell><strong>Date création</strong></TableCell>
              <TableCell><strong>Actions</strong></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {clients.map((client) => (
              <TableRow key={client.id}>
                <TableCell>{client.nomEntreprise}</TableCell>
                <TableCell>{client.siret}</TableCell>
                <TableCell>{client.adresse}</TableCell>
                <TableCell>
                  {new Date(client.dateCreation).toLocaleDateString('fr-FR')}
                </TableCell>
                <TableCell>
                  <IconButton color="primary" onClick={() => handleEdit(client)}>
                    <Edit />
                  </IconButton>
                  <IconButton color="error" onClick={() => handleDelete(client.id)}>
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
            {clients.length === 0 && (
              <TableRow>
                <TableCell colSpan={5} align="center">
                  Aucun client pour le moment
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
}
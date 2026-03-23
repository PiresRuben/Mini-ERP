import { useState } from 'react';
import api from '../api';
import { Box, TextField, Button, Paper, Typography } from '@mui/material';

export default function ClientForm({ client, onClose }) {
  const isEdit = client !== null;

  const [form, setForm] = useState({
    nomEntreprise: client?.nomEntreprise || '',
    siret: client?.siret || '',
    adresse: client?.adresse || '',
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEdit) {
        await api.put(`/api/clients/${client.id}`, form);
      } else {
        await api.post('/api/clients', form);
      }
      onClose(); // Fermer le formulaire et recharger
    } catch (error) {
      console.error('Erreur sauvegarde:', error);
    }
  };

  return (
    <Paper sx={{ p: 3, mb: 3 }}>
      <Typography variant="h6" sx={{ mb: 2 }}>
        {isEdit ? 'Modifier le client' : 'Nouveau client'}
      </Typography>

      <Box component="form" onSubmit={handleSubmit}>
        <TextField
          label="Nom de l'entreprise"
          name="nomEntreprise"
          value={form.nomEntreprise}
          onChange={handleChange}
          fullWidth
          required
          sx={{ mb: 2 }}
        />
        <TextField
          label="SIRET"
          name="siret"
          value={form.siret}
          onChange={handleChange}
          fullWidth
          required
          sx={{ mb: 2 }}
        />
        <TextField
          label="Adresse"
          name="adresse"
          value={form.adresse}
          onChange={handleChange}
          fullWidth
          required
          sx={{ mb: 2 }}
        />

        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button type="submit" variant="contained" color="primary">
            {isEdit ? 'Modifier' : 'Ajouter'}
          </Button>
          <Button variant="outlined" onClick={onClose}>
            Annuler
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
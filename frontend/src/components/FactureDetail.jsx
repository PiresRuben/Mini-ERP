import { useState, useEffect } from 'react';
import api from '../api';
import {
  Container, Typography, Button, Table, TableBody, TableCell,
  TableContainer, TableHead, TableRow, Paper, IconButton, Box,
  Chip, FormControl, InputLabel, Select, MenuItem, TextField,
  Grid, Divider
} from '@mui/material';
import { Delete, ArrowBack } from '@mui/icons-material';

export default function FactureDetail({ factureId, onBack }) {
  const [facture, setFacture] = useState(null);
  const [produits, setProduits] = useState([]);
  const [newLigne, setNewLigne] = useState({
    produitId: '',
    quantite: 1,
    remise: 0,
  });

  useEffect(() => {
    loadFacture();
    loadProduits();
  }, []);

  const loadFacture = async () => {
    try {
      const response = await api.get(`/api/factures/${factureId}`);
      setFacture(response.data);
    } catch (error) {
      console.error('Erreur chargement facture:', error);
    }
  };

  const loadProduits = async () => {
    try {
      const response = await api.get('/api/produits');
      setProduits(response.data);
    } catch (error) {
      console.error('Erreur chargement produits:', error);
    }
  };

  const handleAddLigne = async () => {
    try {
      const response = await api.post(`/api/factures/${factureId}/lignes`, {
        produitId: newLigne.produitId,
        quantite: parseInt(newLigne.quantite),
        remise: parseFloat(newLigne.remise),
      });
      setFacture(response.data);
      setNewLigne({ produitId: '', quantite: 1, remise: 0 });
    } catch (error) {
      console.error('Erreur ajout ligne:', error);
    }
  };

  const handleDeleteLigne = async (ligneId) => {
    try {
      const response = await api.delete(
        `/api/factures/${factureId}/lignes/${ligneId}`
      );
      setFacture(response.data);
    } catch (error) {
      console.error('Erreur suppression ligne:', error);
    }
  };

  const handleStatutChange = async (newStatut) => {
    try {
      const response = await api.put(`/api/factures/${factureId}/statut`, {
        statut: newStatut,
      });
      setFacture(response.data);
    } catch (error) {
      console.error('Erreur changement statut:', error);
    }
  };

  const handleExportJSON = async () => {
    try {
      const response = await api.get(`/api/factures/${factureId}`);
      const data = response.data;
      const blob = new Blob([JSON.stringify(data, null, 2)], {
        type: 'application/json',
      });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${data.numeroFacture}.json`;
      a.click();
      URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Erreur export:', error);
    }
  };

  const handleExportXML = async () => {
    try {
      const response = await api.get(`/api/factures/${factureId}`);
      const data = response.data;

      let xml = '<?xml version="1.0" encoding="UTF-8"?>\n';
      xml += '<Facture>\n';
      xml += `  <NumeroFacture>${data.numeroFacture}</NumeroFacture>\n`;
      xml += `  <DateEmission>${new Date(data.dateEmission).toISOString().split('T')[0]}</DateEmission>\n`;
      xml += `  <Statut>${data.statut}</Statut>\n`;
      xml += `  <Client>${data.nomClient}</Client>\n`;
      xml += '  <Lignes>\n';
      data.lignes.forEach((l) => {
        xml += '    <Ligne>\n';
        xml += `      <Reference>${l.referenceProduit}</Reference>\n`;
        xml += `      <Designation>${l.designationProduit}</Designation>\n`;
        xml += `      <Quantite>${l.quantite}</Quantite>\n`;
        xml += `      <PrixUnitaireHT>${l.prixUnitaireHT.toFixed(2)}</PrixUnitaireHT>\n`;
        xml += `      <Remise>${l.remise.toFixed(2)}</Remise>\n`;
        xml += `      <TotalLigneHT>${l.totalLigneHT.toFixed(2)}</TotalLigneHT>\n`;
        xml += '    </Ligne>\n';
      });
      xml += '  </Lignes>\n';
      xml += `  <TotalHT>${data.totalHT.toFixed(2)}</TotalHT>\n`;
      xml += `  <MontantTVA>${data.montantTVA.toFixed(2)}</MontantTVA>\n`;
      xml += `  <TotalTTC>${data.totalTTC.toFixed(2)}</TotalTTC>\n`;
      xml += '</Facture>';

      const blob = new Blob([xml], { type: 'application/xml' });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${data.numeroFacture}.xml`;
      a.click();
      URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Erreur export XML:', error);
    }
  };

  if (!facture) return <Typography sx={{ m: 4 }}>Chargement...</Typography>;

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      {/* En-tête */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 3 }}>
        <IconButton onClick={onBack}>
          <ArrowBack />
        </IconButton>
        <Typography variant="h4">{facture.numeroFacture}</Typography>
        <Chip label={facture.statut} color="primary" />
      </Box>

      {/* Infos facture */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Grid container spacing={2}>
          <Grid item xs={4}>
            <Typography variant="body2" color="text.secondary">Client</Typography>
            <Typography variant="h6">{facture.nomClient}</Typography>
          </Grid>
          <Grid item xs={4}>
            <Typography variant="body2" color="text.secondary">Date d'émission</Typography>
            <Typography variant="h6">
              {new Date(facture.dateEmission).toLocaleDateString('fr-FR')}
            </Typography>
          </Grid>
          <Grid item xs={4}>
            <Typography variant="body2" color="text.secondary">Statut</Typography>
            <FormControl size="small" sx={{ minWidth: 150 }}>
              <Select
                value={facture.statut}
                onChange={(e) => handleStatutChange(e.target.value)}
              >
                <MenuItem value="Brouillon">Brouillon</MenuItem>
                <MenuItem value="Envoyée">Envoyée</MenuItem>
                <MenuItem value="Payée">Payée</MenuItem>
                <MenuItem value="Annulée">Annulée</MenuItem>
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      {/* Ajouter une ligne */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Typography variant="h6" sx={{ mb: 2 }}>Ajouter une ligne</Typography>
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
          <FormControl sx={{ minWidth: 250 }}>
            <InputLabel>Produit</InputLabel>
            <Select
              value={newLigne.produitId}
              label="Produit"
              onChange={(e) =>
                setNewLigne({ ...newLigne, produitId: e.target.value })
              }
            >
              {produits.map((p) => (
                <MenuItem key={p.id} value={p.id}>
                  {p.designation} — {p.prixUnitaireHT.toFixed(2)}€ HT
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            label="Quantité"
            type="number"
            value={newLigne.quantite}
            onChange={(e) =>
              setNewLigne({ ...newLigne, quantite: e.target.value })
            }
            sx={{ width: 100 }}
            inputProps={{ min: 1 }}
          />
          <TextField
            label="Remise %"
            type="number"
            value={newLigne.remise}
            onChange={(e) =>
              setNewLigne({ ...newLigne, remise: e.target.value })
            }
            sx={{ width: 100 }}
            inputProps={{ min: 0, max: 100 }}
          />
          <Button
            variant="contained"
            onClick={handleAddLigne}
            disabled={!newLigne.produitId}
          >
            Ajouter
          </Button>
        </Box>
      </Paper>

      {/* Tableau des lignes */}
      <TableContainer component={Paper} sx={{ mb: 3 }}>
        <Table>
          <TableHead>
            <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
              <TableCell><strong>Référence</strong></TableCell>
              <TableCell><strong>Désignation</strong></TableCell>
              <TableCell align="right"><strong>Prix HT</strong></TableCell>
              <TableCell align="right"><strong>Quantité</strong></TableCell>
              <TableCell align="right"><strong>Remise</strong></TableCell>
              <TableCell align="right"><strong>Total Ligne HT</strong></TableCell>
              <TableCell></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {facture.lignes.map((ligne) => (
              <TableRow key={ligne.id}>
                <TableCell>{ligne.referenceProduit}</TableCell>
                <TableCell>{ligne.designationProduit}</TableCell>
                <TableCell align="right">
                  {ligne.prixUnitaireHT.toFixed(2)} €
                </TableCell>
                <TableCell align="right">{ligne.quantite}</TableCell>
                <TableCell align="right">{ligne.remise} %</TableCell>
                <TableCell align="right">
                  {ligne.totalLigneHT.toFixed(2)} €
                </TableCell>
                <TableCell>
                  <IconButton
                    color="error"
                    onClick={() => handleDeleteLigne(ligne.id)}
                  >
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
            {facture.lignes.length === 0 && (
              <TableRow>
                <TableCell colSpan={7} align="center">
                  Aucune ligne — ajoutez des produits ci-dessus
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Totaux */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end' }}>
          <Box sx={{ display: 'flex', gap: 4, mb: 1 }}>
            <Typography>Total HT :</Typography>
            <Typography sx={{ minWidth: 100, textAlign: 'right' }}>
              {facture.totalHT.toFixed(2)} €
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', gap: 4, mb: 1 }}>
            <Typography>TVA (20%) :</Typography>
            <Typography sx={{ minWidth: 100, textAlign: 'right' }}>
              {facture.montantTVA.toFixed(2)} €
            </Typography>
          </Box>
          <Divider sx={{ width: 250, my: 1 }} />
          <Box sx={{ display: 'flex', gap: 4 }}>
            <Typography variant="h6">Total TTC :</Typography>
            <Typography variant="h6" sx={{ minWidth: 100, textAlign: 'right' }}>
              {facture.totalTTC.toFixed(2)} €
            </Typography>
          </Box>
        </Box>
      </Paper>

      {/* Boutons export */}
      <Box sx={{ display: 'flex', gap: 2, mb: 4 }}>
        <Button variant="outlined" onClick={handleExportJSON}>
          Exporter JSON
        </Button>
        <Button variant="outlined" onClick={handleExportXML}>
          Exporter XML
        </Button>
      </Box>
    </Container>
  );
}
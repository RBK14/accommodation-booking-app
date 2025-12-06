import React from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box,
    Grid,
    Card,
    CardContent,
    Typography,
    Button
} from '@mui/material';
import PeopleIcon from '@mui/icons-material/People';
import VillaIcon from '@mui/icons-material/Villa';
import { PRIMARY_BLUE, DARK_GRAY, TEXT_WHITE } from '../../assets/styles/colors';

const AdminDashboard = () => {
    const navigate = useNavigate();

    const dashboardItems = [
        {
            title: 'Baza Użytkowników',
            description: 'Przeglądaj, edytuj i zarządzaj kontami użytkowników.',
            icon: <PeopleIcon sx={{ fontSize: 40, color: PRIMARY_BLUE }} />,
            path: '/admin/users',
            btnText: 'Zarządzaj użytkownikami'
        },
        {
            title: 'Baza Ofert',
            description: 'Zarządzaj wszystkimi ofertami dodanymi przez gospodarzy.',
            icon: <VillaIcon sx={{ fontSize: 40, color: PRIMARY_BLUE }} />,
            path: '/admin/offers',
            btnText: 'Zarządzaj ofertami'
        }
    ];

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4" sx={{ mb: 4, fontWeight: 'bold', color: DARK_GRAY }}>
                Panel Administratora
            </Typography>

            <Grid container spacing={3}>
                {dashboardItems.map((item, index) => (
                    <Grid item xs={12} md={6} lg={4} key={index}>
                        <Card
                            sx={{
                                height: '100%',
                                display: 'flex',
                                flexDirection: 'column',
                                transition: 'transform 0.2s, box-shadow 0.2s',
                                '&:hover': {
                                    transform: 'translateY(-4px)',
                                    boxShadow: '0 12px 24px rgba(0, 0, 0, 0.1)',
                                }
                            }}
                        >
                            <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center', gap: 2, pt: 4 }}>
                                {/* Ikona z tłem */}
                                <Box sx={{
                                    p: 2,
                                    borderRadius: '50%',
                                    backgroundColor: 'rgba(13, 110, 253, 0.1)'
                                }}>
                                    {item.icon}
                                </Box>

                                <Typography variant="h5" component="div" sx={{ fontWeight: 'bold' }}>
                                    {item.title}
                                </Typography>

                                <Typography variant="body2" color="text.secondary">
                                    {item.description}
                                </Typography>
                            </CardContent>

                            <Box sx={{ p: 2, mt: 'auto' }}>
                                <Button
                                    fullWidth
                                    variant="contained"
                                    onClick={() => navigate(item.path)}
                                    sx={{
                                        backgroundColor: PRIMARY_BLUE,
                                        color: TEXT_WHITE,
                                        py: 1.5,
                                        '&:hover': {
                                            backgroundColor: '#0a58ca',
                                        }
                                    }}
                                >
                                    {item.btnText}
                                </Button>
                            </Box>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Box>
    );
};

export default AdminDashboard;
import api from './api';

export interface LoginCredentials {
    email: string;
    password: string;
}

export interface AuthResponse {
    Token: string;
    Nom: string;
    Role: string;
}

export interface RegisterCompanyDto {
    nomEntreprise: string;
    matriculeFiscale: string;
    nomResponsable: string;
    emailResponsable: string;
}

export interface User {
    nom: string;
    role: string;
}

export const authService = {
    async login(credentials: LoginCredentials): Promise<AuthResponse> {
        const response = await api.post<any>('/auth/login', credentials);
        const data = response.data;

        // Détecter aussi bien PascalCase (C#) que camelCase (JSON par défaut)
        const finalToken = data.Token || data.token;
        const finalNom = data.Nom || data.nom;
        const finalRole = data.Role || data.role;

        if (finalToken) {
            localStorage.setItem('token', finalToken);
            localStorage.setItem('user', JSON.stringify({
                nom: finalNom || 'Utilisateur',
                role: finalRole || 'Candidat'
            }));
        }
        return response.data;
    },

    async registerCompany(data: RegisterCompanyDto): Promise<{ Message: string }> {
        const response = await api.post<{ Message: string }>('/Registration', data);
        return response.data;
    },

    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
    },

    async completeActivation(token: string, password: string): Promise<{ Message: string }> {
        const response = await api.post<{ Message: string }>('/Activation/complete', {
            token,
            password
        });
        return response.data;
    },

    getCurrentUser(): User | null {
        try {
            const userStr = localStorage.getItem('user');
            if (!userStr) return null;

            const data = JSON.parse(userStr);
            // Toujours retourner un objet avec 'nom' et 'role' en minuscules
            return {
                nom: data.nom || data.Nom || 'Utilisateur',
                role: data.role || data.Role || 'Candidat'
            };
        } catch (e) {
            localStorage.removeItem('user');
            return null;
        }
    }
};

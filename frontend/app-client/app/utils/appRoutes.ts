/**
 * APP_ROUTES - Constantes de rutas para la aplicación Kasumi CRM
 * 
 * Centraliza todas las rutas utilizadas en la aplicación.
 */

// Objeto principal que contiene todas las categorías de rutas
export const APP_ROUTES = {
  /**
   * Rutas públicas - accesibles sin autenticación
   */
  PUBLIC: {
    LOGIN: '/',
    SIGNUP: '/signup',
    FORGOT_PASSWORD: '/forgot-password',
    RESET_PASSWORD: (token: string) => `/reset-password/${token}`,
  },

  /**
   * Rutas protegidas - todas bajo /home
   */
  HOME: (() => {
    const BASE = '/home';

    return {
      // Ruta base para el dashboard
      BASE,

      // Ruta de overview (índice de home)
      OVERVIEW: BASE,

      /**
       * Rutas de embudos (funnels)
       */
      FUNNELS: (() => {
        const FUNNELS_BASE = `${BASE}/funnels`;

        return {
          BASE: FUNNELS_BASE,
          LIST: FUNNELS_BASE,
          CREATE: `${FUNNELS_BASE}/create`,
          DETAIL: (id: string) => `${FUNNELS_BASE}/${id}`,
        };
      })(),

      CUSTOMERS: (() => {
        const CUSTOMERS_BASE = `${BASE}/customers`;
        return {
          BASE: CUSTOMERS_BASE,
          LIST: CUSTOMERS_BASE,
          CREATE: `${CUSTOMERS_BASE}/create`,
          DETAIL: (id: string) => `${CUSTOMERS_BASE}/${id}`,
        };
      })(),

      CHAT: (() => {
        const CHAT_BASE = `${BASE}/chat`;
        return {
          BASE: CHAT_BASE,
          TALK: (id: string) => `${CHAT_BASE}/${id}`,
        };
      })(),
      /**
       * Rutas de análisis
       */
      SELLER_PERFORMANCE: `${BASE}/seller-performance`,
      FUNNEL_PERFORMANCE: `${BASE}/funnel-performance`,
      CUSTOMER_WORKLOAD: `${BASE}/customer-workload`,
      /**
       * Rutas de administración
       */
      USER_MANAGEMENT: `${BASE}/user-management`,
      SETTINGS: `${BASE}/settings`,
    };
  })(),
};

// Exportación por defecto para facilitar importaciones
export default APP_ROUTES;
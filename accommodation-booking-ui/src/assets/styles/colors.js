// Primary Colors
export const PRIMARY_BLUE = '#0d6efd';
export const DARK_GRAY = '#212529';
export const LIGHT_GRAY = '#f1f3f5';

// Secondary Colors
export const TEXT_WHITE = '#ffffff';
export const TEXT_DARK = '#212529';
export const BORDER_GRAY = '#adb5bd';
export const HOVER_GRAY = '#495057';
export const SUBTLE_GRAY = '#95a5a6';
export const LIGHT_BG = '#f8f9fa';

// Semantic Colors
export const SUCCESS = '#198754';
export const WARNING = '#ffc107';
export const DANGER = '#dc3545';
export const INFO = '#0dcaf0';

// Gradient
export const GRADIENT_PURPLE = 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)';

// Export color palette for easy access
export const colors = {
  primary: {
    blue: PRIMARY_BLUE,
  },
  background: {
    light: LIGHT_GRAY,
    dark: DARK_GRAY,
    white: TEXT_WHITE,
    lightBg: LIGHT_BG,
  },
  text: {
    white: TEXT_WHITE,
    dark: TEXT_DARK,
    muted: SUBTLE_GRAY,
  },
  border: {
    gray: BORDER_GRAY,
    hover: HOVER_GRAY,
  },
  semantic: {
    success: SUCCESS,
    warning: WARNING,
    danger: DANGER,
    info: INFO,
  },
  gradient: {
    purple: GRADIENT_PURPLE,
  },
};

export default colors;

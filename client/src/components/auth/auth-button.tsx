import styles from './styles/auth-button.module.css'

// this interface is used to define the props that the AuthButton component will receive
interface AuthButtonProps {
    children: React.ReactNode
    disabled?: boolean
    icon: React.ReactNode
    onClick?: () => void
}

export function AuthButton({ children, disabled = false, icon, onClick }: AuthButtonProps) {
    return (
        <button
            className={`${styles.button} ${disabled ? styles.disabled : styles.active}`}
            disabled={disabled}
            onClick={onClick}
        >
            {icon}
            <span className={styles.buttonText}>{children}</span>
        </button>
    )
}
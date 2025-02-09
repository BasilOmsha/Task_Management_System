import { cn } from "@/lib/utils.ts"

import styles from "./styles/form-Input.module.css"

interface FormInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
    error?: string
    label: string
}

export function FormInput({ className, error, label, ...props }: FormInputProps) {
    return (
        <div className={styles.container}>
            <label className={styles.label}>
                {label}
            </label>
            <input
                className={cn(
                    styles.input,
                    error && styles.error,
                    className
                )}
                {...props}
            />
            {error && (
                <p className={styles.errorMessage}>{error}</p>
            )}
        </div>
    )
}